#!/usr/bin/env python3

import rclpy
from rclpy.node import Node

from sensor_msgs.msg import Image, LaserScan
from nav_msgs.msg import Odometry
from tf2_msgs.msg import TFMessage

class RelayNode(Node):
    def __init__(self):
        super().__init__('relay_node')

        # Subscriptions
        self.create_subscription(Image, '/camera/depth_raw_unity', self.depth_cb, 10)
        self.create_subscription(Image, '/camera/image_raw_unity', self.image_cb, 10)
        self.create_subscription(Odometry, '/odom_unity', self.odom_cb, 10)
        self.create_subscription(TFMessage, '/tf_unity', self.tf_cb, 10)
        self.create_subscription(LaserScan, '/scan_unity', self.scan_cb, 10)

        # Publishers
        self.depth_pub = self.create_publisher(Image, '/camera/depth_raw', 10)
        self.image_pub = self.create_publisher(Image, '/camera/image_raw', 10)
        self.odom_pub = self.create_publisher(Odometry, '/odom', 10)
        self.tf_pub = self.create_publisher(TFMessage, '/tf', 10)
        self.scan_pub = self.create_publisher(LaserScan, '/scan', 10)

    def depth_cb(self, msg):
        self.depth_pub.publish(msg)

    def image_cb(self, msg):
        self.image_pub.publish(msg)

    def odom_cb(self, msg):
        self.odom_pub.publish(msg)

    def tf_cb(self, msg):
        self.tf_pub.publish(msg)

    def scan_cb(self, msg):
        self.scan_pub.publish(msg)

def main(args=None):
    rclpy.init(args=args)
    node = RelayNode()
    rclpy.spin(node)
    node.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()

