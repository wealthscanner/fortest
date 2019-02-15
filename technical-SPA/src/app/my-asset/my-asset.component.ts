import { Component, OnInit } from '@angular/core';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { AuthService } from '../_service/auth.service';
import { UserService } from '../_service/user.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-my-asset',
  templateUrl: './my-asset.component.html',
  styleUrls: ['./my-asset.component.scss']
})
export class MyAssetComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  sellParam: string;
  userParams: any = {};

  constructor(private authService: AuthService, private userService: UserService,
    private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
    this.sellParam = 'Assets';

    this.userParams.OlderThanDays = 0;
    this.userParams.Gender = 'collection';
    this.loadUsers();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    console.log(this.userParams.Gender);

    this.userService.getUsers(this.pagination.currentPage,
      this.pagination.itemsPerPage, this.userParams, this.sellParam)
      .subscribe((res: PaginatedResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }
}
