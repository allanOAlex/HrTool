output "vpc-id" {
  value       = module.vpc.env-vpc.id
  description = "Environment VPC ID"
}

output "public_subnets" {
  value = module.subnet.public_subnets
}

output "private_subnets" {
  value = module.subnet.private_subnets
}