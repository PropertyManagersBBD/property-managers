resource "aws_iam_policy" "bucket_access_policy" {
  name = "property-manager-backend-storage-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "s3:*"
        Effect = "Allow"
        Resource = [
          "arn:aws:s3:::property-manager-backend-file-storage",
          "arn:aws:s3:::property-manager-backend-storage/*"
        ]
      },
    ]
  })
}

resource "aws_iam_role" "beanstalk_ec2" {
  assume_role_policy = jsonencode(
    {
      Version = "2012-10-17"
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "ec2.amazonaws.com"
          }
        },
      ]
    }
  )
  description           = "Allows EC2 instances to call AWS services on your behalf."
  force_detach_policies = false
  managed_policy_arns = [
    aws_iam_policy.bucket_access_policy.arn,
    "arn:aws:iam::aws:policy/AWSElasticBeanstalkMulticontainerDocker",
    "arn:aws:iam::aws:policy/AWSElasticBeanstalkWebTier",
    "arn:aws:iam::aws:policy/AWSElasticBeanstalkWorkerTier"
  ]
  max_session_duration = 3600
  name                 = "aws-elasticbeanstalk-ec2"
  path                 = "/"
}

data "aws_secretsmanager_secret_version" "propertymanager-db-details" {
  secret_id = module.rds.db_instance_master_user_secret_arn
}

resource "aws_iam_instance_profile" "beanstalk_ec2" {
  name = "aws-elasticbeanstalk-ec2-profile"
  role = aws_iam_role.beanstalk_ec2.name
}

resource "aws_elastic_beanstalk_application" "beanstalk_app" {
  name        = "property-manager-backend-app"
  description = "Backend for Property Managers"
}

resource "aws_elastic_beanstalk_environment" "beanstalk_env" {
  name                = "property-manager-backend-env"
  application         = aws_elastic_beanstalk_application.beanstalk_app.name
  solution_stack_name = "64bit Amazon Linux 2023 v3.0.5 running .NET 6"
  tier                = "WebServer"

  setting {
    namespace = "aws:ec2:vpc"
    name      = "VPCId"
    value     = module.vpc.vpc_id
  }
  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "IamInstanceProfile"
    value     = aws_iam_instance_profile.beanstalk_ec2.name
  }
  setting {
    namespace = "aws:ec2:vpc"
    name      = "Subnets"
    value     = join(",", module.vpc.public_subnets)
  }
  setting {
    namespace = "aws:ec2:instances"
    name      = "InstanceTypes"
    value     = "t3.micro"
  }
  setting {
    namespace = "aws:elasticbeanstalk:healthreporting:system"
    name      = "SystemType"
    value     = "basic"
  }
  setting {
    namespace = "aws:elasticbeanstalk:application"
    name      = "Application Healthcheck URL"
    value     = "/"
  }
  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "Timeout"
    value     = "60"
  }
  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "IgnoreHealthCheck"
    value     = "true"
  }
  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "SingleInstance"
  }
  setting {
    namespace = "aws:elasticbeanstalk:managedactions"
    name      = "ManagedActionsEnabled"
    value     = "false"
  }
  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "DB_CONNECTION_STRING"
    value     = "Data Source=${module.rds.db_instance_address};Initial Catalog=PropertyManager;User ID=dbuser;Password=${jsondecode(data.aws_secretsmanager_secret_version.propertymanager-db-details.secret_string)["password"]};Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}


