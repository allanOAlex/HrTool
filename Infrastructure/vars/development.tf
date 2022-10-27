locals {
  development = {
    ecr_url_ui            = "623800556929.dkr.ecr.us-east-1.amazonaws.com/hrtool-dev-ui:latest" 
    ecr_url            = "623800556929.dkr.ecr.us-east-1.amazonaws.com/hrtool-dev:latest" 
    ecs_environment     = "Development"
    cidr_block         = "10.30.0.0/16"
    private_subnets    = ["10.30.0.0/19", "10.30.32.0/19"]
    public_subnets     = ["10.30.128.0/20", "10.30.144.0/20"]
    availability_zones = ["us-east-1a", "us-east-1b"]
    eip                = ["eipalloc-0672a502796adbfe8","eipalloc-03cf1c056a960c7a2"]
    project ="hrtool"
  }
}