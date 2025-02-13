import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {TranslatePipe} from '@ngx-translate/core';

@Component({
  selector: 'app-search',
  imports: [FormsModule, TranslatePipe],
  templateUrl: './search.component.html',
  standalone: true,
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  public query: string = "";

  constructor(private router: Router) {
  }

  public onSubmit() {
    if (this.query.trim()) {
      // this.router.navigateByUrl(`/search/${this.query}`);
    } else {
    }
    this.query = "";
  }
}
