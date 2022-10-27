variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}

variable "public_subnets" {
  description = "Array of public subnets"
}

variable "private_subnets" {
  description = "Array of private subnets"
}

variable "vpc-id" {
  description = "Environment VPC ID"
}

variable "eip"{
  description = "EIP For NAT Gateway"
}