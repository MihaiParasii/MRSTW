import { Component } from '@angular/core';
import {TranslatePipe} from '@ngx-translate/core';
import {SearchComponent} from '../search/search.component';

@Component({
  selector: 'app-action-bar',
  imports: [
    TranslatePipe,
    SearchComponent
  ],
  templateUrl: './action-bar.component.html',
  standalone: true,
  styleUrl: './action-bar.component.css'
})
export class ActionBarComponent {

}
