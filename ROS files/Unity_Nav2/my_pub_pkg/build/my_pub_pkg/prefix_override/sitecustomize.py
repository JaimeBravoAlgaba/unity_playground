import sys
if sys.prefix == '/usr':
    sys.real_prefix = sys.prefix
    sys.prefix = sys.exec_prefix = '/home/lorenzo/Desktop/CAR/Unity_Nav2/my_pub_pkg/install/my_pub_pkg'
