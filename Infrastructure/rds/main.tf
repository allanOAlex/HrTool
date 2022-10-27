resource "aws_db_subnet_group" "rds-subnet-group" {
  name       = "${var.project}-${var.environment}-rds-subnet-group"
  subnet_ids = var.rds_subnets.*.id
  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-rds-subnet-group"
  }
}

resource "aws_db_instance" "db-rds" {
  identifier             = "${var.project}-${var.environment}-rds"
  name                   = "${var.project}${var.environment}"
  instance_class         = "db.t3.micro"
  allocated_storage      = 20
  engine                 = "postgres"
  engine_version         = "13.4"
  skip_final_snapshot    = true
  publicly_accessible    = false
  vpc_security_group_ids = [var.security-group-rds-id]
  username               = "postgres"
  password               = "RHg,=wz7+8HyjgFSH9Sp"

  db_subnet_group_name = aws_db_subnet_group.rds-subnet-group.name
  
   tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-rds"
  }
}
