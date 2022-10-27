resource "aws_nat_gateway" "main" {
  count         = length(var.private_subnets)
  subnet_id     = element(var.public_subnets.*.id, count.index)
  allocation_id = element(var.eip, count.index)
  depends_on    = [aws_internet_gateway.main]
  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.environment}-${var.project}-nat${format("%03d", count.index + 1)}"
  }
}

resource "aws_internet_gateway" "main" {
  vpc_id = var.vpc-id
  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.environment}-${var.project}-internet-gateway"
  }
}