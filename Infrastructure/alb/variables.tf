variable "public_subnets" {
  description = "Array of public subnets"
}
variable "security-group-alb-id" {
  description = "ALB Security Group"
}
variable "security-group-ecs-tasks-id" {
  description = "ALB Security Group"
}

variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}

variable "vpc-id" {
  description = "Environment VPC ID"
}

variable "private_subnets" {
  description = "Array of private subnets"
}

variable "name" {}