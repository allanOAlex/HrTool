resource "aws_vpc" "env-vpc" {
  cidr_block = var.cidr_block #"10.0.0.0/16"
  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.environment}-vpc-${var.project}"
  }
}

resource "aws_vpc_dhcp_options" "env-vpc-dhcp" {
  domain_name         = "ec2.internal"
  domain_name_servers = ["AmazonProvidedDNS"]
  tags = {
    Name        = "${var.environment}-vpc_dhcp_options-${var.project}"
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
  }
}

resource "aws_vpc_dhcp_options_association" "env-dns-resolver" {
  vpc_id          = aws_vpc.env-vpc.id
  dhcp_options_id = aws_vpc_dhcp_options.env-vpc-dhcp.id
}


