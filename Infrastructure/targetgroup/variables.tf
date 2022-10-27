variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}


variable "alb_tls_cert_arn" {
  description = "CERT ARN"
}

variable "name" {}

variable "health_check_path" {}

variable "aws_lb_id" {
  
}

  variable "vpc-id" {
  
}

variable "path-pattern" {}

variable "port" {
  
}