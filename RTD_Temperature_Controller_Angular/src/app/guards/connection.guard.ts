import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const connectionGuard: CanActivateFn = (route, state) => {
  
  const router: Router = inject(Router);
  if (localStorage.getItem("connected") == "true")
    return true;
  
  alert("Permission denied")
  router.navigateByUrl("/")
  return false;
};