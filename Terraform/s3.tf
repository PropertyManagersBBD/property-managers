resource "aws_s3_bucket" "beanstalk_release_bucket" {
  bucket        = "property-manager-backend-deploy-bucket"
  force_destroy = true
}

resource "aws_s3_bucket" "beanstalk_file_bucket" {
  bucket        = "property-manager-backend-storage"
  force_destroy = true
}

resource "aws_s3_bucket" "frontend_beanstalk_release_bucket" {
  bucket        = "property-manager-frontend-deploy-bucket"
  force_destroy = true
}
