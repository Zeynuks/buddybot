import { useRef } from "react";
import defaultUserPhoto from "../../assets/default-user-photo.jpg";
import { ALLOWED_MEDIA_EXTENSIONS, ALLOWED_MEDIA_TYPES, MAX_MEDIA_FILE_SIZE_BYTES } from "../../constants/media";
import { getMediaUrl } from "../../services/mediaService";

type Props = {
    mediaId: string;
    setMediaId: (id: string) => void;
    mediaUploading: boolean;
    setMediaUploading: (value: boolean) => void;
    mediaUploadError: string | null;
    setMediaUploadError: (error: string | null) => void;
    onUpload: (file: File) => Promise<string>;
};

export function MediaUploader({
    mediaId,
    setMediaId,
    mediaUploading,
    setMediaUploading,
    mediaUploadError,
    setMediaUploadError,
    onUpload,
}: Props) {
    const inputRef = useRef<HTMLInputElement>(null);

    const mediaUrl = mediaId ? getMediaUrl(mediaId) : "";
    const isVideo = /\.(mp4|webm|ogg)$/i.test(mediaUrl);

    const handleInputClick = () => inputRef.current?.click();

    const handleUpload = async (file: File) => {
        if (!ALLOWED_MEDIA_TYPES.includes(file.type)) {
            setMediaUploadError(`Недопустимый формат. Разрешены: ${ALLOWED_MEDIA_EXTENSIONS.join(", ")}`);
            return;
        }
        if (file.size > MAX_MEDIA_FILE_SIZE_BYTES) {
            setMediaUploadError("Файл слишком большой. Максимум — 50 МБ.");
            return;
        }

        setMediaUploading(true);
        setMediaUploadError(null);

        try {
            const id = await onUpload(file);
            setMediaId(id);
        } catch {
            setMediaUploadError("Ошибка загрузки файла");
            setMediaId("");
        } finally {
            setMediaUploading(false);
        }
    };

    return (
        <div className="d-flex flex-column align-items-center border border-2 rounded-4 py-4 px-3 bg-light border-secondary-subtle">
            {mediaId ? (
                isVideo ? (
                    <video
                        src={mediaUrl}
                        className="rounded-circle border mb-3"
                        style={{ width: 140, height: 140, objectFit: "cover" }}
                        muted
                        playsInline
                        autoPlay
                        loop
                    />
                ) : (
                    <img
                        src={mediaUrl}
                        alt="Медиа"
                        className="mb-3 rounded-3 border"
                        style={{ maxWidth: 360, maxHeight: 480 }}
                    />
                )
            ) : (
                <img
                    src={defaultUserPhoto}
                    alt="Заглушка"
                    className="mb-3 rounded-3 border"
                    style={{ maxWidth: 360, maxHeight: 480 }}
                />
            )}

            <input
                ref={inputRef}
                type="file"
                accept="image/*,video/*"
                hidden
                onChange={(e) => {
                    const file = e.target.files?.[0];
                    if (file) handleUpload(file);
                }}
                disabled={mediaUploading}
            />

            <div className="d-flex gap-2 mt-2">
                <button
                    type="button"
                    className="btn btn-outline-secondary btn-sm px-3"
                    onClick={handleInputClick}
                    disabled={mediaUploading}
                >
                    Изменить
                </button>
                <button
                    type="button"
                    className="btn btn-danger btn-sm px-3"
                    onClick={() => setMediaId("")}
                    disabled={mediaUploading || !mediaId}
                >
                    Удалить
                </button>
            </div>

            <div className="form-text mt-2 text-center small text-muted">
                Форматы: <strong>{ALLOWED_MEDIA_EXTENSIONS.join(", ")}</strong>
                <br />
                Размер до <strong>{Math.floor(MAX_MEDIA_FILE_SIZE_BYTES / 1024 / 1024)} МБ</strong>
            </div>

            {mediaUploading && <div className="small text-primary mt-2">Загрузка файла...</div>}
            {mediaUploadError && <div className="alert alert-danger mt-2 py-2 small">{mediaUploadError}</div>}
        </div>
    );
}
