provider "aws" {
  region  = "us-east-1"
  profile = "gsg"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-hrtool"
    key     = "state/terraform.tfstate"
    region  = "us-east-1"
    encrypt = true
  }
}


#
# Fetch variables based on Environment
#
module "vars" {
  source      = "./vars"
  environment = terraform.workspace
}

module "network" {
  source             = "./network"
  cidr_block         = module.vars.env.cidr_block
  environment        = terraform.workspace
  project            = module.vars.env.project
  private_subnets    = module.vars.env.private_subnets
  public_subnets     = module.vars.env.public_subnets
  availability_zones = module.vars.env.availability_zones
  eip                = module.vars.env.eip
}

module "security-group" {
  source      = "./security-group"
  vpc-id      = module.network.vpc-id
  environment = terraform.workspace
  project     = module.vars.env.project
}

module "alb" {
  name = "${terraform.workspace}-${module.vars.env.project}"
  source                      = "./alb"
  public_subnets              = module.network.public_subnets
  security-group-alb-id       = module.security-group.security-group-alb-id
  security-group-ecs-tasks-id = module.security-group.security-group-ecs-tasks-id
  vpc-id                      = module.network.vpc-id
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  private_subnets             = module.network.private_subnets
}

module "iam" {
  source      = "./iam"
  environment = terraform.workspace
  project     = module.vars.env.project
}


module "rds" {
  source                = "./rds"
  project               = module.vars.env.project
  environment           = terraform.workspace
  security-group-rds-id = module.security-group.security-group-rds-id
  rds_subnets           = module.network.private_subnets
}

module "vpn" {
  source                = "./instance"
  project               = module.vars.env.project
  environment           = terraform.workspace
  vpc_security_group_id = module.security-group.security-group-vpn-id
  vpc_id                = module.network.vpc-id
  public_subnet_id      = module.network.public_subnets.*.id[0]
  keyname               = "gsg-dev"
  ami                   = "ami-07c69bbfb09e2ae03"
}


module "ecs-cluster" {
  source ="./cluster"
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  name = "${terraform.workspace}-${module.vars.env.project}"
}


module "alb-tagetgroup" {
  port=80
    vpc-id                      = module.network.vpc-id
    health_check_path = "/swagger/index.html"
  source ="./targetgroup"
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  name = "${terraform.workspace}-${module.vars.env.project}"
   alb_tls_cert_arn            = "arn:aws:acm:us-east-1:623800556929:certificate/a55e5233-d888-4c0a-b359-fc955e3b25d6"
      aws_lb_id = module.alb.aws_lb_id
      path-pattern = "/api/*"
}

module "alb-tagetgroup-ui" {
  port=80
    vpc-id                      = module.network.vpc-id
    health_check_path = "/"
  source ="./targetgroup"
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  name = "${terraform.workspace}-${module.vars.env.project}-ui"
   alb_tls_cert_arn            = "arn:aws:acm:us-east-1:623800556929:certificate/a55e5233-d888-4c0a-b359-fc955e3b25d6"
      aws_lb_id = module.alb.aws_lb_id
            path-pattern = "/*"
}











resource "aws_alb_listener" "https" {
  load_balancer_arn = module.alb.aws_lb_id
  port              = 443
  protocol          = "HTTPS"
 
  ssl_policy        = "ELBSecurityPolicy-2016-08"
  certificate_arn   = "arn:aws:acm:us-east-1:623800556929:certificate/a55e5233-d888-4c0a-b359-fc955e3b25d6"
 
  default_action {
    target_group_arn = module.alb-tagetgroup-ui.alb_target_group-id
    type             = "forward"
  }
}

resource "aws_lb_listener_rule" "static" {
  listener_arn = aws_alb_listener.https.arn
  priority     = 10

  action {
    target_group_arn = module.alb-tagetgroup.alb_target_group-id
    type             = "forward"
  }

  condition {
    path_pattern {
      values = ["/api/*"]
    }
  }
}















module "application-ui" {
  name = "${terraform.workspace}-${module.vars.env.project}-ui"
  source                      = "./application"
  ecs_environment             = module.vars.env.ecs_environment
  private_subnets             = module.network.private_subnets
  iam-ecr-role-arn            = module.iam.iam-ecr-role-arn
  public_subnets              = module.network.public_subnets
  security-group-ecs-tasks-id = module.security-group.security-group-ecs-tasks-id
  alb_target_group-id         = module.alb-tagetgroup-ui.alb_target_group-id
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  ecr_url                     = module.vars.env.ecr_url_ui
  hostPort= 80
  containerPort = 80
  aws_ecs_cluster = module.ecs-cluster.aws_ecs_cluster
}


module "application" {
  name = "${terraform.workspace}-${module.vars.env.project}"
  source                      = "./application"
  ecs_environment             = module.vars.env.ecs_environment
  private_subnets             = module.network.private_subnets
  iam-ecr-role-arn            = module.iam.iam-ecr-role-arn
  public_subnets              = module.network.public_subnets
  security-group-ecs-tasks-id = module.security-group.security-group-ecs-tasks-id
  alb_target_group-id         = module.alb-tagetgroup.alb_target_group-id
  environment                 = terraform.workspace
  project                     = module.vars.env.project
  ecr_url                     = module.vars.env.ecr_url
  hostPort= 8080
  containerPort = 8080
    aws_ecs_cluster = module.ecs-cluster.aws_ecs_cluster
}
