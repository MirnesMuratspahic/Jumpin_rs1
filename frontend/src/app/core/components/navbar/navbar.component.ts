import { Component } from '@angular/core';
import { navbarLinks } from '../../../constants/navbarLinks';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterModule],
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  navbarLinks = navbarLinks;
  isNavbarCollapsed = true;
  userRole: string | null = localStorage.getItem('role');

  getfiltredLinks() {
    return this.navbarLinks.filter(
      (link) => !link.role || link.role === this.userRole
    );
  }

  toggleNavbar() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
  }

  handleLinkClick(link: any) {
    if (link.onClick) {
      link.onClick();
    }
  }
}
