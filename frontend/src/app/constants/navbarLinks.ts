export const navbarLinks = [
  {
    title: 'Home',
    href: '/home',
  },
  {
    title: 'Profile',
    href: '/profile',
  },
  {
    title: 'Requests',
    href: '/requests',
  },
  {
    title: 'Support',
    href: '/support',
  },
  {
    title: 'Admin',
    href: '/admin',
    role: 'Admin',
  },
  {
    title: 'Logout',
    href: '/',
    onClick: () => {
      localStorage.removeItem('token');
      localStorage.removeItem('role');
    },
  },
];
