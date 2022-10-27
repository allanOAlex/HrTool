
resource "aws_alb_target_group" "main" {
  name        = "${var.name}-tg"
  port        = var.port
  protocol    = "HTTP"
  vpc_id      = var.vpc-id
  target_type = "ip"

  health_check {
    healthy_threshold   = "3"
    interval            = "30"
    protocol            = "HTTP"
    matcher             = "200"
    timeout             = "3"
    path                = var.health_check_path
    unhealthy_threshold = "2"
  }

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Name        = "${var.name}-tg"
    Terraform   = "true"
  }
}

output "alb_target_group-id" {
  value = aws_alb_target_group.main.id
}

