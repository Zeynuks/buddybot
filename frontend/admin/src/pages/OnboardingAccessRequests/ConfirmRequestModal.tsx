import type { OnboardingAccessRequest } from "../../models/OnboardingAccessRequests";
import type { DepartmentLookup } from "../../models/Department";
import type { TeamLookup } from "../../models/Teams";
import type { UserLookup } from "../../models/User";
import { useState, useEffect } from "react";
import { getDepartmentsLookup } from "../../services/departmentService";
import { getTeamsLookup } from "../../services/teamService";
import { getUsersLookup } from "../../services/userService";
import { ru } from "date-fns/locale";
import LoadingSpinner from "../../components/LoadingSpinner/LoadingSpinner";
import DatePicker from "react-datepicker";
import Modal from "react-bootstrap/Modal";
import dayjs from "dayjs";
import "react-datepicker/dist/react-datepicker.css";
import "dayjs/locale/ru";

type ConfirmRequestModalProps = {
    open: boolean;
    onClose: () => void;
    request: OnboardingAccessRequest | null;
    onConfirm: (values: { onboardingAccessTimeUtc?: string; teamId: number; mentorIds: number[] }) => void;
};

export default function ConfirmRequestModal({ open, onClose, request, onConfirm }: ConfirmRequestModalProps) {
    const [onboardingAccessTime, setOnboardingAccessTime] = useState<Date | null>(null);
    const [departmentId, setDepartmentId] = useState<number | "">("");
    const [teamId, setTeamId] = useState<number | "">("");
    const [mentorIds, setMentorIds] = useState<number[]>([]);
    const [departments, setDepartments] = useState<DepartmentLookup[]>([]);
    const [teams, setTeams] = useState<TeamLookup[]>([]);
    const [mentors, setMentors] = useState<UserLookup[]>([]);
    const [initialLoading, setInitialLoading] = useState(true);

    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!open) return;
        setInitialLoading(true);
        Promise.all([getDepartmentsLookup(), getUsersLookup({ roles: ["Mentor"] })])
            .then(([departments, mentors]) => {
                setDepartments(departments);
                setMentors(mentors);

                const accessTime = request?.onboardingAccessTime
                    ? new Date(request.onboardingAccessTime)
                    : (() => {
                          const now = new Date();
                          now.setMinutes(0, 0, 0);
                          now.setHours(now.getHours() + 1);
                          return now;
                      })();

                setOnboardingAccessTime(accessTime);
                setDepartmentId(request?.department?.id ?? "");
                setTeamId(request?.team?.id ?? "");
                setMentorIds(request?.mentors?.map((m) => m.id) ?? []);
            })
            .finally(() => setInitialLoading(false));
    }, [open, request]);

    useEffect(() => {
        if (!open || !departmentId) {
            setTeams([]);
            setTeamId("");
            return;
        }
        setInitialLoading(true);
        getTeamsLookup(Number(departmentId))
            .then((result) => setTeams(result))
            .catch(() => setTeams([]))
            .finally(() => setInitialLoading(false));
    }, [departmentId, open]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!onboardingAccessTime) {
            setError("Выберите дату и время начала онбординга");
            return;
        }
        if (onboardingAccessTime.getTime() <= Date.now()) {
            setError("Дата и время начала онбординга не может быть в прошлом");
            return;
        }

        if (teamId && mentorIds.length) {
            onConfirm({
                onboardingAccessTimeUtc: onboardingAccessTime ? onboardingAccessTime.toISOString() : undefined,
                teamId: Number(teamId),
                mentorIds,
            });
        }
    };

    return (
        <Modal show={open} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Подтверждение заявки на онбординг</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {initialLoading ? (
                    <div className="d-flex justify-content-center py-4">
                        <LoadingSpinner />
                    </div>
                ) : request ? (
                    <form onSubmit={handleSubmit}>
                        <div className="d-flex align-items-center mb-2 flex-wrap">
                            <div className="fw-semibold">
                                {request.firstName} {request.lastName}
                            </div>
                        </div>
                        <div className="small text-secondary mb-2">
                            <i className="bi bi-calendar me-1"></i>
                            Создана: {dayjs(request.createdAt).format("D MMM YYYY HH:mm")}
                        </div>
                        <div className="mb-3">
                            <label className="form-label fs-6 mb-1 required">
                                Дата и время начала <span className="text-muted me-2">(время по МСК)</span>
                            </label>
                            <DatePicker
                                showIcon
                                icon="bi bi-calendar"
                                selected={onboardingAccessTime}
                                onChange={setOnboardingAccessTime}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={15}
                                dateFormat="dd.MM.yyyy HH:mm"
                                locale={ru}
                                className="form-control rounded-3"
                                placeholderText="Выберите дату и время"
                                minDate={new Date()}
                                minTime={
                                    onboardingAccessTime &&
                                    onboardingAccessTime.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setMinutes(new Date().getMinutes() + 1))
                                        : undefined
                                }
                                maxTime={
                                    onboardingAccessTime &&
                                    onboardingAccessTime.toDateString() === new Date().toDateString()
                                        ? new Date(new Date().setHours(23, 59, 0, 0))
                                        : undefined
                                }
                                autoComplete="off"
                                timeCaption="Время"
                            />
                        </div>
                        <div className="mb-3">
                            <label className="form-label required">Отдел</label>
                            <select
                                className="form-select rounded-3"
                                value={departmentId}
                                onChange={(e) => {
                                    setDepartmentId(Number(e.target.value) || "");
                                    setTeamId("");
                                }}
                                required
                            >
                                <option value="">Выберите отдел</option>
                                {departments.map((dep) => (
                                    <option key={dep.id} value={dep.id}>
                                        {dep.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label required">Команда</label>
                            <select
                                className="form-select rounded-3"
                                value={teamId}
                                onChange={(e) => setTeamId(Number(e.target.value) || "")}
                                required
                                disabled={!departmentId}
                            >
                                <option value="">Выберите команду</option>
                                {teams.map((team) => (
                                    <option key={team.id} value={team.id}>
                                        {team.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label required">Наставник</label>
                            <select
                                className="form-select rounded-3"
                                value={mentorIds[0] ?? ""}
                                onChange={(e) => setMentorIds([Number(e.target.value)])}
                                required
                            >
                                <option value="">Выберите наставника</option>
                                {mentors.map((mentor) => (
                                    <option key={mentor.id} value={mentor.id}>
                                        {mentor.firstName} {mentor.lastName}
                                    </option>
                                ))}
                            </select>
                        </div>
                        {error && <div className="alert alert-danger mb-3">{error}</div>}
                        <div className="d-flex gap-2 mt-4">
                            <button
                                className="btn btn-primary flex-fill py-2"
                                type="submit"
                                disabled={!onboardingAccessTime || !teamId || !mentorIds.length}
                            >
                                Подтвердить
                            </button>
                            <button className="btn btn-light flex-fill py-2" type="button" onClick={onClose}>
                                Отмена
                            </button>
                        </div>
                    </form>
                ) : (
                    <div className="text-center">Заявка не найдена</div>
                )}
            </Modal.Body>
        </Modal>
    );
}
