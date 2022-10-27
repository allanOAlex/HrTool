variable "vpc-id" {
  description = "Environment VPC ID"
}

variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}