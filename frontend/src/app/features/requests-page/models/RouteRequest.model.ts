import { Request } from "./request.model";
import { UserRoute } from "../../home-page/models/userRoute";

export interface RouteRequest extends Request {
    userRoute: UserRoute
}