module "vpc" {
  source      = "./vpc"
  cidr_block  = var.cidr_block
  environment = var.environment
  project     = var.project
}

module "subnet" {
  source             = "./subnet"
  vpc-id             = module.vpc.env-vpc.id
  private_subnets    = var.private_subnets
  public_subnets     = var.public_subnets
  environment        = var.environment
  project            = var.project
  availability_zones = var.availability_zones
}


module "nat" {
  source          = "./nat"
  vpc-id          = module.vpc.env-vpc.id
  public_subnets  = module.subnet.public_subnets
  private_subnets = var.private_subnets
  environment     = var.environment
  project         = var.project
  eip             = var.eip
}

module "routing" {
  source           = "./routing"
  vpc-id           = module.vpc.env-vpc.id
  private_subnets  = module.subnet.private_subnets
  public_subnets   = module.subnet.public_subnets
  environment      = var.environment
  project          = var.project
  nat_gateway      = module.nat.nat_gateway
  internet_gateway = module.nat.internet_gateway
}
