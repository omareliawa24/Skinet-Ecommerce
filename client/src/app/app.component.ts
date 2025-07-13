import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

 sayHello() {
    alert('أهلاً بيك يا عمر 🌟');
  }

  protected title = 'client';
}
