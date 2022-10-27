variable "vpc-id" {
  description = "Environment VPC ID"
}

variable "private_subnets" {
  description = "Array of private subnets"
}

variable "public_subnets" {
  description = "Array of public subnets"
}

variable "availability_zones" {
  description = "Availability Zones"
}

variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}