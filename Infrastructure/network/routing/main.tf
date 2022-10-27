resource "aws_route_table" "private" {
  count  = length(var.private_subnets)
  vpc_id = var.vpc-id
  tags = {
    Name      = "${var.environment}-${var.project}-routing-table-private-${format("%03d", count.index + 1)}"
    terraform = "true"
  }
}

resource "aws_route_table_association" "private" {
  count          = length(var.private_subnets)
  subnet_id      = element(var.private_subnets.*.id, count.index)
  route_table_id = element(aws_route_table.private.*.id, count.index)
}

resource "aws_route" "private" {
  count = length(var.private_subnets)

  route_table_id         = element(aws_route_table.private.*.id, count.index)
  destination_cidr_block = "0.0.0.0/0"
  nat_gateway_id         = element(var.nat_gateway.*.id, count.index)
}


resource "aws_route_table" "public" {
  vpc_id = var.vpc-id
  tags = {
    Name        = "${var.environment}-${var.project}-routing-table-public-001"
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
  }
}

resource "aws_route_table_association" "public" {
  count          = length(var.public_subnets)
  subnet_id      = element(var.public_subnets.*.id, count.index)
  route_table_id = aws_route_table.public.id
}

resource "aws_route" "public" {
  route_table_id         = aws_route_table.public.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = var.internet_gateway.id
}
