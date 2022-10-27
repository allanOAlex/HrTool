output "nat_gateway" {
  value = aws_nat_gateway.main
}

output "internet_gateway" {
  value = aws_internet_gateway.main
}