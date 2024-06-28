module "rds" {
  source = "terraform-aws-modules/rds/aws"

  identifier                  = "propertymanager"
  engine                      = "sqlserver-ex"
  engine_version              = "15.00"
  family                      = "sqlserver-ex-15.0" # DB parameter group
  major_engine_version        = "15.00"             # DB option group
  instance_class              = "db.t3.micro"
  allocated_storage           = 20
  deletion_protection         = false
  skip_final_snapshot         = true
  db_subnet_group_name        = module.vpc.database_subnet_group_name
  vpc_security_group_ids      = [aws_security_group.rds.id]
  publicly_accessible         = true
  storage_encrypted           = false
  create_db_instance          = true
  port                        = 1433
  username                    = "dbuser"
  manage_master_user_password = true
}
