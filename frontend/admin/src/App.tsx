import { Routes, Route } from "react-router-dom";
import AuthLayout from "./layouts/AuthLayout";
import { Suspense, lazy } from "react";
import { ROUTES } from "./constants/routes";
import MainLayout from "./layouts/MainLayout";

const Login = lazy(() => import("./pages/Auth/Login"));

const Dashboard = lazy(() => import("./pages/Dashboard/Dashboard"));

const Departments = lazy(() => import("./pages/Departments/Departments"));
const CreateDepartment = lazy(() => import("./pages/Departments/CreateDepartment"));
const EditDepartment = lazy(() => import("./pages/Departments/EditDepartment"));

const Countries = lazy(() => import("./pages/Countries/Countries"));
const CreateCountry = lazy(() => import("./pages/Countries/CreateCountry"));
const EditCountry = lazy(() => import("./pages/Countries/EditCountry"));

const Cities = lazy(() => import("./pages/Cities/Cities"));
const CreateCity = lazy(() => import("./pages/Cities/CreateCity"));
const EditCity = lazy(() => import("./pages/Cities/EditCity"));

const Teams = lazy(() => import("./pages/Teams/Teams"));
const CreateTeam = lazy(() => import("./pages/Teams/CreateTeam"));
const EditTeam = lazy(() => import("./pages/Teams/EditTeam"));

const Members = lazy(() => import("./pages/Members/Members"));
const CreateMentor = lazy(() => import("./pages/Members/CreateMentor"));
const EditMember = lazy(() => import("./pages/Members/EditMember"));

const Invitations = lazy(() => import("./pages/Invitations/Invitations"));
const CreateInvitationCandidate = lazy(() => import("./pages/Invitations/CreateInvitationCandidate"));
const CreateInvitationHR = lazy(() => import("./pages/Invitations/CreateInvitationHR"));
const EditInvitation = lazy(() => import("./pages/Invitations/EditInvitation"));

const InvitationDetails = lazy(() => import("./pages/Invitations/InvitationDetailsPage"));

const Candidates = lazy(() => import("./pages/Candidates/Candidates"));
const EditCandidate = lazy(() => import("./pages/Candidates/EditCandidate"));

const OnboardingAccessRequests = lazy(() => import("./pages/OnboardingAccessRequests/OnboardingAccessRequests"));
const EditOnboardingAccessRequest = lazy(() => import("./pages/OnboardingAccessRequests/EditOnboardingAccessRequest"));

const Feedbacks = lazy(() => import("./pages/Feedbacks/Feedbacks"));
const Test = lazy(() => import("./pages/Test/Test"));

import { PrivateRoute } from "./components/PrivateRoute/PrivateRoute";
import LoadingSpinner from "./components/LoadingSpinner/LoadingSpinner";

export default function App() {
    return (
        <Suspense fallback={<LoadingSpinner />}>
            <Routes>
                <Route
                    path={ROUTES.AUTH.LOGIN}
                    element={
                        <AuthLayout>
                            <Login />
                        </AuthLayout>
                    }
                />

                <Route element={<PrivateRoute />}>
                    <Route element={<MainLayout />}>
                        <Route path={ROUTES.DASHBOARD} element={<Dashboard />} />
                        <Route path={ROUTES.DEPARTMENTS.ROOT} element={<Departments />} />
                        <Route path={ROUTES.DEPARTMENTS.CREATE} element={<CreateDepartment />} />
                        <Route path={ROUTES.DEPARTMENTS.EDIT(":id")} element={<EditDepartment />} />
                        <Route path={ROUTES.COUNTRIES.ROOT} element={<Countries />}></Route>
                        <Route path={ROUTES.COUNTRIES.CREATE} element={<CreateCountry />} />
                        <Route path={ROUTES.COUNTRIES.EDIT(":id")} element={<EditCountry />} />
                        <Route path={ROUTES.CITIES.BY_COUNTRY(":countryId")} element={<Cities />} />
                        <Route path={ROUTES.CITIES.CREATE(":countryId")} element={<CreateCity />} />
                        <Route path={ROUTES.CITIES.EDIT(":countryId", ":id")} element={<EditCity />} />
                        <Route path={ROUTES.TEAMS.ROOT} element={<Teams />} />
                        <Route path={ROUTES.TEAMS.CREATE} element={<CreateTeam />} />
                        <Route path={ROUTES.TEAMS.EDIT(":id")} element={<EditTeam />} />
                        <Route path={ROUTES.MEMBERS.ROOT} element={<Members />} />
                        <Route path={ROUTES.MEMBERS.CREATE_MENTOR} element={<CreateMentor />} />
                        <Route path={ROUTES.MEMBERS.EDIT(":id")} element={<EditMember />} />
                        <Route path={ROUTES.INVITATIONS.ROOT} element={<Invitations />} />
                        <Route path={ROUTES.INVITATIONS.CREATE_CANDIDATE} element={<CreateInvitationCandidate />} />
                        <Route path={ROUTES.INVITATIONS.CREATE_HR} element={<CreateInvitationHR />} />
                        <Route path={ROUTES.INVITATIONS.DETAIL(":token")} element={<InvitationDetails />} />
                        <Route path={ROUTES.INVITATIONS.EDIT(":token")} element={<EditInvitation />} />
                        <Route path={ROUTES.CANDIDATES.ROOT} element={<Candidates />} />
                        <Route path={ROUTES.CANDIDATES.EDIT(":id")} element={<EditCandidate />} />
                        <Route path={ROUTES.ONBOARDING_ACCESS_REQUESTS.ROOT} element={<OnboardingAccessRequests />} />
                        <Route
                            path={ROUTES.ONBOARDING_ACCESS_REQUESTS.EDIT(":id")}
                            element={<EditOnboardingAccessRequest />}
                        />
                        <Route path={ROUTES.FEEDBACKS.ROOT} element={<Feedbacks />} />
                    </Route>
                </Route>

                <Route path={ROUTES.TEST} element={<Test />} />
            </Routes>
        </Suspense>
    );
}
