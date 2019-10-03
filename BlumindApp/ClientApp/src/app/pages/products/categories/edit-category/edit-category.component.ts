import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';
import { CategoryEdit } from 'src/app/_models/categoryEdit';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) {
  }
  title = 'Category';
  selectedProducts;
  products = [];
  model = new CategoryEdit();
  ngOnInit() {
    this.http.get(API_ENDPOINT + 'Product/GetAllProducts').toPromise().then(result => {
      this.products = result as [{}];
    });
  }
  addedProduct = (value) => ({ name: value });

  public submit() {
    console.log(this.model);
    this.http.post(API_ENDPOINT + 'Category/PostCategory', this.model)
      .toPromise().then(() => {
        // this.router.navigate(['countries']);
      });
  }

}
