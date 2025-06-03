import sys
if sys.prefix == '/usr':
    sys.real_prefix = sys.prefix
    sys.prefix = sys.exec_prefix = '/home/lorenzo/Bureau/Unity_Playground/install/ros_tcp_endpoint'
