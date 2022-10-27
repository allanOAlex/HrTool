output "security-group-alb-id" {
  value = aws_security_group.alb.id
}

output "security-group-ecs-tasks-id" {
  value = aws_security_group.ecs_tasks.id
}

output "security-group-rds-id" {
  value = aws_security_group.db-sg.id
}

output "security-group-vpn-id" {
  value = aws_security_group.vpn_access_server.id
}
