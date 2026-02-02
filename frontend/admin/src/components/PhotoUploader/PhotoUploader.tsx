import { useRef } from "react";
import defaultUserPhoto from "../../assets/default-user-photo.jpg";
import { ALLOWED_PHOTO_EXTENSIONS, ALLOWED_PHOTO_TYPES, MAX_MEDIA_FILE_SIZE_BYTES } from "../../constants/media";
import { getMediaUrl } from "../../services/mediaService";

type Props = {
    photoId: string;
    setPhotoId: (id: string) => void;
    photoUploading: boolean;
    setPhotoUploading: (value: boolean) => void;
    photoUploadError: string | null;
    setPhotoUploadError: (error: string | null) => void;
    onUpload: (file: File) => Promise<string>;
};

export function PhotoUploader({
    photoId,
    setPhotoId,
    photoUploading,
    setPhotoUploading,
    photoUploadError,
    setPhotoUploadError,
    onUpload,
}: Props) {
    const inputRef = useRef<HTMLInputElement>(null);

    const photoUrl = photoId ? getMediaUrl(photoId) : "";

    const handleInputClick = () => inputRef.current?.click();

    const handleUpload = async (file: File) => {
        if (!ALLOWED_PHOTO_TYPES.includes(file.type)) {
            setPhotoUploadError(`Недопустимый формат. Разрешены: ${ALLOWED_PHOTO_EXTENSIONS.join(", ")}`);
            return;
        }

        if (file.size > MAX_MEDIA_FILE_SIZE_BYTES) {
            setPhotoUploadError("Файл слишком большой. Максимум — 50 МБ.");
            return;
        }

        setPhotoUploading(true);
        setPhotoUploadError(null);

        try {
            const id = await onUpload(file);
            setPhotoId(id);
        } catch {
            setPhotoUploadError("Ошибка загрузки файла");
            setPhotoId("");
        } finally {
            setPhotoUploading(false);
        }
    };

    return (
        <div className="d-flex flex-column align-items-center border border-2 rounded-4 py-4 px-3 bg-light border-secondary-subtle">
            <img
                src={photoUrl || defaultUserPhoto}
                alt="Фото"
                className="mb-3 rounded-3 border"
                style={{ maxWidth: 360, maxHeight: 480 }}
            />

            <input
                ref={inputRef}
                type="file"
                accept="image/*"
                hidden
                onChange={(e) => {
                    const file = e.target.files?.[0];
                    if (file) handleUpload(file);
                }}
                disabled={photoUploading}
            />

            <div className="d-flex gap-2 mt-2">
                <button
                    type="button"
                    className="btn btn-outline-secondary btn-sm px-3"
                    onClick={handleInputClick}
                    disabled={photoUploading}
                >
                    Изменить
                </button>
                <button
                    type="button"
                    className="btn btn-danger btn-sm px-3"
                    onClick={() => setPhotoId("")}
                    disabled={photoUploading || !photoId}
                >
                    Удалить
                </button>
            </div>

            <div className="form-text mt-2 text-center small text-muted">
                Форматы: <strong>{ALLOWED_PHOTO_EXTENSIONS.join(", ")}</strong>
                <br />
                Размер до <strong>{Math.floor(MAX_MEDIA_FILE_SIZE_BYTES / 1024 / 1024)} МБ</strong>
            </div>

            {photoUploading && <div className="small text-primary mt-2">Загрузка фото...</div>}
            {photoUploadError && <div className="alert alert-danger mt-2 py-2 small">{photoUploadError}</div>}
        </div>
    );
}
