import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {NavbarComponent} from '../navbar/navbar.component';
import {FooterComponent} from '../footer/footer.component';
import {SidebarCategoriesComponent} from '../../../user-ui/components/sidebar-categories/sidebar-categories.component';
import {ActionBarComponent} from '../action-bar/action-bar.component';

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent,
    SidebarCategoriesComponent,
    ActionBarComponent
  ],
  templateUrl: './layout.component.html',
  standalone: true,
  styleUrl: './layout.component.css'
})
export class LayoutComponent {

}
