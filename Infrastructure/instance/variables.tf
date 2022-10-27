variable "vpc_security_group_id" {}

variable "vpc_id" {}

variable "public_subnet_id" {}

variable "keyname" {
  description = "A pre configured EC2 SSH key pair"
}

variable "ami" {
  description = "This is the AMI for the most recent version of OpenVPN access server with 10 connected devices"
  default = "ami-07c69bbfb09e2ae03"
}


variable "environment" {
  type        = string
  description = "Current Working environment"
}

variable "project" {
  type        = string
  description = "Project Name"
}
