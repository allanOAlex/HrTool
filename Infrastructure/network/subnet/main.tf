resource "aws_subnet" "private" {
  vpc_id            = var.vpc-id
  cidr_block        = element(var.private_subnets, count.index)
  availability_zone = element(var.availability_zones, count.index)
  count             = length(var.private_subnets)

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.environment}-${var.project}-private-subnet-${format("%03d", count.index + 1)}"
  }
}

resource "aws_subnet" "public" {
  vpc_id            = var.vpc-id
  cidr_block        = element(var.public_subnets, count.index)
  availability_zone = element(var.availability_zones, count.index)
  count             = length(var.public_subnets)

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.environment}-${var.project}-public-subnet-${format("%03d", count.index + 1)}"
  }
}

