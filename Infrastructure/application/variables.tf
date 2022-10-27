variable "iam-ecr-role-arn" {
  description = "Array of public subnets"
}
variable "public_subnets" {
  description = "Array of public subnets"
}
variable "security-group-ecs-tasks-id" {
  description = "Array of public subnets"
}
variable "alb_target_group-id" {

}
variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}
variable "private_subnets" {
  description = "Array of private subnets"
}

variable "ecs_environment"{
  description = "The type of config environment for ECS task to run from"
}

variable "ecr_url"{
  description = "URL For ECR"
}

variable "name"{}

variable "hostPort"{}

variable "containerPort"{}

variable "aws_ecs_cluster" {}