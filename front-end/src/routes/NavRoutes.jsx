import Home from "../pages/Home";
import ManageUser from "../pages/users/AdminUser/ManageUser";
import ManageAssets from "../pages/assets/ManageAssets";
import ReportPage from "../pages/report/ReportPage";
import RequestForReturningPage from "../pages/request/RequestForReturning";
// import ReportPage from "../pages/report/ReportPage";

export const NavRoutes = [
    {
        path: "/",
        element: <Home />,
        title: "Home",
      },
      {
        path: "/user",
        element: <ManageUser />,
        title: "Manage User",
      },
      {
        path: "/asset",
        element: <ManageAssets />,
        title: "Manage Asset",
      },
      {
        path: "/assignment",
    
        title: "Manage Assignment",
      },
      {
        path: "/request",
        element: <RequestForReturningPage/>,
        title: "Request for Returning",
      },
    
      {
        path: "/report",
        element:<ReportPage/>,
        title: "Report",
      },
    
];