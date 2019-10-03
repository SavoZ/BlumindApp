import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CategoryEdit } from 'src/app/_models/categoryEdit';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) {
  }

  model = new CategoryEdit();
  ngOnInit() {
  }

  public submit() {
    console.log(this.model);
    this.http.post(API_ENDPOINT + 'Category/PostCategory', this.model)
      .toPromise().then(() => {
        // this.router.navigate(['countries']);
      });
  }

}
