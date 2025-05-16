import { Component } from '@angular/core';
import {TranslatePipe} from '@ngx-translate/core';
import {SearchComponent} from '../search/search.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-action-bar',
  imports: [
    TranslatePipe,
    SearchComponent,
    RouterLink
  ],
  templateUrl: './action-bar.component.html',
  standalone: true,
  styleUrl: './action-bar.component.css'
})
export class ActionBarComponent {

}
