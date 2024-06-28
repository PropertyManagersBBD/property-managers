data "tls_certificate" "githubActions" {
  url = "https://token.actions.githubusercontent.com"
}

resource "aws_iam_openid_connect_provider" "githubActions" {
  url             = "https://token.actions.githubusercontent.com"
  client_id_list  = ["sts.amazonaws.com"]
  thumbprint_list = distinct(concat(data.tls_certificate.githubActions.certificates[*].sha1_fingerprint, ["6938fd4d98bab03faadb97b34396831e3780aea1", "1c58a3a8518e8759bf075b76b750d4f2df264fcd", "1b511abead59c6ce207077c0bf0e0043b1382612"]))
}
locals {
  trust_policy = jsonencode(
    {
      "Version" : "2012-10-17",
      "Statement" : [
        {
          "Effect" : "Allow",
          "Principal" : {
            "Federated" : "arn:aws:iam::535492056399:oidc-provider/token.actions.githubusercontent.com"
          },
          "Action" : "sts:AssumeRoleWithWebIdentity",
          "Condition" : {
            "StringEquals" : {
              "token.actions.githubusercontent.com:aud" : "sts.amazonaws.com"
            },
            "StringLike" : {
              "token.actions.githubusercontent.com:sub" : "repo:bbd-miniconomy-PropertyManager/*:*"
            }
          }
        }
      ]
    }
  )
}
resource "aws_iam_role" "githubActions" {
  name                 = "GithubRunnerRole"
  assume_role_policy   = local.trust_policy
  managed_policy_arns  = ["arn:aws:iam::aws:policy/AdministratorAccess"]
  max_session_duration = 3600
}

output "githubActionsRole" {
  value = aws_iam_role.githubActions.arn
}

