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

variable "public_subnets" {
  description = "Array of public subnets"
}

variable "vpc-id" {
  description = "Environment VPC ID"
}

variable "nat_gateway" {
  description = "Subnet Nat Gateway"
}

variable "internet_gateway" {
  description = "Subnet Internet Gateway"
}