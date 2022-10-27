resource "aws_ecs_service" "main" {
  name                               = "${var.name}-ecs-service"
  cluster                            = var.aws_ecs_cluster
  task_definition                    = aws_ecs_task_definition.main.arn
  desired_count                      = 1
  deployment_minimum_healthy_percent = 50
  deployment_maximum_percent         = 200
  launch_type                        = "FARGATE"
  scheduling_strategy                = "REPLICA"
  health_check_grace_period_seconds  = 600

  network_configuration {
    security_groups  = [var.security-group-ecs-tasks-id]
    subnets          = var.private_subnets.*.id
    assign_public_ip = false
  }

  load_balancer {
    target_group_arn = var.alb_target_group-id
    container_name   = "${var.name}-container"
    container_port   = var.containerPort
  }

  lifecycle {
    ignore_changes = [task_definition, desired_count]
  }
}


resource "aws_ecs_task_definition" "main" {
  network_mode             = "awsvpc"
  family                   = "${var.name}-family"
  requires_compatibilities = ["FARGATE"]
  cpu                      = 1024
  memory                   = 8192
  execution_role_arn       = var.iam-ecr-role-arn
  container_definitions = jsonencode([{
    name      = "${var.name}-container"
    image     = "${var.ecr_url}"
    essential = true
    environment = [
      { "name" : "ASPNETCORE_ENVIRONMENT", "value" : "${var.ecs_environment}" }
    ]
    portMappings = [{
      protocol      = "tcp"
      containerPort = var.containerPort
      hostPort      = var.hostPort
    }],
    "logConfiguration" : {
      "logDriver" : "awslogs",
      "secretOptions" : null,
      "options" : {
        "awslogs-group" : "${var.environment}-${var.project}-main",
        "awslogs-region" : "us-east-1",
        "awslogs-stream-prefix" : "web"
      }
    } }
  ])
}

resource "aws_cloudwatch_log_group" "main" {
  name = "${var.name}-main"

  tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Name        = "${var.name}-main"
    Terraform   = "true"
  }
}