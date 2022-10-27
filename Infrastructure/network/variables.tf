variable "cidr_block" {
  type        = string
  description = "VPC cidr block"
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

variable "public_subnets" {
  description = "Array of public subnets"
}

variable "availability_zones" {
  description = "Availability Zones"
}

variable "eip"{
  description = "EIP For NAT Gateway"
}