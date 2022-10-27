resource "aws_security_group" "alb" {
  name   = "${var.project}-${var.environment}-sg-alb-A"
  vpc_id = var.vpc-id

  ingress {
    protocol         = "tcp"
    from_port        = 80
    to_port          = 80
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  ingress {
    protocol         = "tcp"
    from_port        = 443
    to_port          = 443
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  egress {
    protocol         = "-1"
    from_port        = 0
    to_port          = 0
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  tags = {
    Name        = "${var.project}-${var.environment}-sg-alb-A"
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
  }
}

resource "aws_security_group" "ecs_tasks" {
  name   = "${var.project}-${var.environment}-sg-task-A"
  vpc_id = var.vpc-id

  ingress {
    protocol        = "tcp"
    from_port       = 0
    to_port         = 65535
    security_groups = [aws_security_group.alb.id]
  }

  egress {
    protocol         = "-1"
    from_port        = 0
    to_port          = 0
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-sg-task-A"
  }
}


resource "aws_security_group" "db-sg" {
  name   = "${var.project}-${var.environment}-sg-rds"
  vpc_id = var.vpc-id
  description = "Allow all inbound for Postgres"

  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    protocol         = "-1"
    from_port        = 0
    to_port          = 0
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-sg-rds"
  }
}

resource "aws_security_group" "vpn_access_server" {
  name        = "${var.project}-${var.environment}-sg-vpn"
  description = "Security group for VPN access server"
  vpc_id = var.vpc-id

  ingress {
    protocol  = "tcp"
    from_port = 443
    to_port   = 443
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  egress {
    protocol         = "-1"
    from_port        = 0
    to_port          = 0
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }


  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-sg-vpn"
  }
}