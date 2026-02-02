type Props = {
    value: string;
    onChange: (val: string) => void;
    placeholder?: string;
};

export default function SearchInput({ value, onChange, placeholder }: Props) {
    return (
        <input
            type="text"
            className="form-control bg-light rounded-pill px-3"
            placeholder={placeholder}
            value={value}
            onChange={(e) => onChange(e.target.value)}
        />
    );
}
