import { Routes, Route } from "react-router-dom";
export default function RouteComponent({ routes }) {
    return (<Routes>
           {routes.map(route =>
            <Route key={route.path} path={route.path} element={route.element}/>
            )}
    </Routes>
    )


}

