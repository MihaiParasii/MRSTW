import { Component } from '@angular/core';
import {ActionBarComponent} from '../../../../common-ui/components/action-bar/action-bar.component';
import {FooterComponent} from '../../../../common-ui/components/footer/footer.component';
import {NavbarComponent} from '../../../../common-ui/components/navbar/navbar.component';
import {RouterOutlet} from '@angular/router';
import {
  SidebarCategoriesComponent
} from '../../../../common-ui/components/sidebar-categories/sidebar-categories.component';
import {AdminSidebarComponent} from '../admin-sidebar/admin-sidebar.component';

@Component({
  selector: 'app-admin-layout',
  imports: [
    ActionBarComponent,
    FooterComponent,
    NavbarComponent,
    RouterOutlet,
    SidebarCategoriesComponent,
    AdminSidebarComponent
  ],
  templateUrl: './admin-layout.component.html',
  standalone: true,
  styleUrl: './admin-layout.component.css'
})
export class AdminLayoutComponent {

}
