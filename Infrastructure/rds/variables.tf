variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}

variable "security-group-rds-id" {
  description = "RDS Security Group"
}

variable "rds_subnets" {
  description = "Array of private subnets"
}