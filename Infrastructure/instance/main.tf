resource "aws_instance" "vpn_access_server" {
  ami                         = "${var.ami}"
  instance_type               = "t2.nano"
  vpc_security_group_ids      = ["${var.vpc_security_group_id}"]
  associate_public_ip_address = true
  subnet_id                   = "${var.public_subnet_id}"
  key_name                    = "${var.keyname}"

   tags = {
    Project     = "${var.project}"
    Environment = "${var.environment}"
    Terraform   = "true"
    Name        = "${var.project}-${var.environment}-vpn"
  }
}