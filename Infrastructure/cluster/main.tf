
resource "aws_ecs_cluster" "ecs-cluster" {
  name = "${var.environment}-${var.project}-ecs"
  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Name        = "${var.environment}-${var.project}-ecs"
    Terraform   = "true"
  }
}

output "aws_ecs_cluster" {
  value = aws_ecs_cluster.ecs-cluster.id
}
