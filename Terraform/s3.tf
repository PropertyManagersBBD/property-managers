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

resource "aws_s3_bucket_public_access_block" "frontend-public-access" {
  bucket = aws_s3_bucket.frontend_beanstalk_release_bucket.id

  block_public_acls       = false
  block_public_policy     = false
  ignore_public_acls      = false
  restrict_public_buckets = false
}


resource "aws_s3_bucket_policy" "frontend-bucket-policy" {
  bucket = aws_s3_bucket.frontend_beanstalk_release_bucket.id
  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Sid" : "PublicReadGetObject",
        "Effect" : "Allow",
        "Principal" : "*",
        "Action" : "s3:GetObject",
        "Resource" : "${aws_s3_bucket.frontend_beanstalk_release_bucket.arn}/*"
      }
    ]
  })
}

resource "aws_s3_bucket_website_configuration" "frontend-website-configuration" {
  bucket = aws_s3_bucket.frontend_beanstalk_release_bucket.id
  index_document {
    suffix = "index.html"
  }
}



