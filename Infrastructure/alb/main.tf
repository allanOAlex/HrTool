
resource "aws_lb" "main" {
  name               = "${var.name}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [var.security-group-alb-id]
  subnets            = var.public_subnets.*.id

  enable_deletion_protection = false

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Name        = "${var.name}-alb"
    Terraform   = "true"
  }
}


# Redirect to https listener
resource "aws_alb_listener" "http" {
  load_balancer_arn = aws_lb.main.id
  port              = 80
  protocol          = "HTTP"
 
  default_action {
   type = "redirect"
 
   redirect {
     port        = 443
     protocol    = "HTTPS"
     status_code = "HTTP_301"
   }
  }
}


output "aws_lb_id" {
  value = aws_lb.main.id
}